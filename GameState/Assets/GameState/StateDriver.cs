using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class StateDriver : StateMachineBehaviour
    {
        [Header("OnStateEnter")]
        [SerializeField] Incident _incidentOnEnter;
        [SerializeField] bool _skipIncidentFirstTimeOnEnter;
        private bool _isFirstTimeOnEnter = true;
        [SerializeField] List<ParameterSetInfo> _parametersOnEnter = new List<ParameterSetInfo>();
        
        [Header("OnStateExit")]
        [SerializeField] Incident _incidentOnExit;
        [SerializeField] bool _skipIncidentFirstTimeOnExit;
        private bool _isFirstTimeOnExit = true;
        [SerializeField] List<ParameterSetInfo> _parametersOnExit = new List<ParameterSetInfo>();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < _parametersOnEnter.Count; i++)
            {
                var para = _parametersOnEnter[i];
                if (para.Hash == 0)
                    para.Hash = Animator.StringToHash(para.Name);
                switch (para.Type)
                {
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(para.Hash, para.BooleanValue);
                        break;
                    case AnimatorControllerParameterType.Int:
                        animator.SetInteger(para.Hash, para.IntegerValue);
                        break;
                    case AnimatorControllerParameterType.Float:
                        animator.SetFloat(para.Hash, para.FloatValue);
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        if (para.TriggerValue)
                            animator.SetTrigger(para.Hash);
                        else
                            animator.ResetTrigger(para.Hash);
                        break;
                }
            }

            if (_skipIncidentFirstTimeOnEnter && _isFirstTimeOnEnter)
                _isFirstTimeOnEnter = false;
            else
                _incidentOnEnter?.Invoke(this);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < _parametersOnExit.Count; i++)
            {
                var para = _parametersOnExit[i];
                if (para.Hash == 0)
                    para.Hash = Animator.StringToHash(para.Name);
                switch (para.Type)
                {
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(para.Hash, para.BooleanValue);
                        break;
                    case AnimatorControllerParameterType.Int:
                        animator.SetInteger(para.Hash, para.IntegerValue);
                        break;
                    case AnimatorControllerParameterType.Float:
                        animator.SetFloat(para.Hash, para.FloatValue);
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        if (para.TriggerValue)
                            animator.SetTrigger(para.Hash);
                        else
                            animator.ResetTrigger(para.Hash);
                        break;
                }
            }

            if (_skipIncidentFirstTimeOnExit && _isFirstTimeOnExit)
                _isFirstTimeOnExit = false;
            else
                _incidentOnExit?.Invoke(this);
        }
    }
}
