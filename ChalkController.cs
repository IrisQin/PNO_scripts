namespace VRTK.Examples
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ChalkController : VRTK_InteractableObject
    {

        public BlackboardControllerCopy blackboard;
        public Rigidbody rigidbody;
        private RaycastHit touch;
        private bool lastTouch;
        private MeshCollider deadbody;
        private bool first;
        private bool voIsPlayed;
        private Plane[] planes = new Plane[6];
        public Camera cam;

        [FMODUnity.EventRef]
        public string chalkEvent;
        FMOD.Studio.EventInstance chalk;

        [FMODUnity.EventRef]
        public string reactionVO;
        FMOD.Studio.EventInstance reactionVOInstance;

        // Use this for initialization
        void Start()
        {
            deadbody = GameObject.FindGameObjectWithTag("DeadBody").GetComponent<MeshCollider>();
            //this.blackboard = GameObject.Find("Blackboard").GetComponent<BlackboardController>();
            //this.blackboard = GameObject.Find("Ground").GetComponent<BlackboardController>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            float tipHeight = transform.Find("Tip").transform.localScale.y - 0.33f;
            Vector3 tip = transform.Find("Tip").transform.position;

            if (Physics.Raycast(tip, transform.up, out touch, tipHeight))
            {
                if (!(touch.collider.tag == "Ground"))
                {
                    return;
                }
                this.blackboard = touch.collider.GetComponent<BlackboardControllerCopy>();
                Debug.Log("Touching!");

                this.blackboard.SetColor(new Color(1,1,1,1));
                this.blackboard.SetTouchPosition(touch.textureCoord.x, touch.textureCoord.y);
                this.blackboard.ToggleTouch(true);

                rigidbody.freezeRotation = true;

                if (!lastTouch)
                {
                    lastTouch = true;
                    chalk = FMODUnity.RuntimeManager.CreateInstance(chalkEvent);
                    chalk.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    chalk.start();
                }

            }
            else
            {
                if(lastTouch)
                    this.blackboard.ToggleTouch(false);

                rigidbody.freezeRotation = false;
                chalk.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                chalk.release();
                lastTouch = false;
            }

            if(this.IsGrabbed() && FrustumCheckerCharacter() && !voIsPlayed)
            {
                StartCoroutine(StareTimerCharacter());
            }

        }

        private bool FrustumCheckerCharacter()
        {
            planes = GeometryUtility.CalculateFrustumPlanes(cam);

            if (!first)
            {
                if (GeometryUtility.TestPlanesAABB(planes, deadbody.bounds))
                {
                    return true;
                }
            }
            else
                first = false;
            
            return false;
        }

        private void PlayVO()
        {
            reactionVOInstance = FMODUnity.RuntimeManager.CreateInstance(reactionVO);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(reactionVOInstance, this.transform, this.GetComponent<Rigidbody>());
            reactionVOInstance.start();
        }

        IEnumerator StareTimerCharacter()
        {
            yield return new WaitForSeconds(1.5f);
            if (FrustumCheckerCharacter() && !voIsPlayed)
            {
                PlayVO();
                voIsPlayed = true;
            }
        }
    }
}
