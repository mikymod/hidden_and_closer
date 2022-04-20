using UnityEngine;
using UnityEditor;

namespace HNC
{
    [CustomEditor(typeof(DetectionSystem))]
    public class DetectionSystemEditor : Editor
    {
        private void OnSceneGUI()
        {
            DetectionSystem detection = (DetectionSystem)target;

            
            Handles.color = Color.yellow;
            Handles.DrawWireArc(detection.transform.position, Vector3.up, Vector3.forward, 360, detection.hearingRadius);

            
            Handles.color = Color.magenta;
            Handles.DrawWireArc(detection.transform.position, Vector3.up, Vector3.forward, 360, detection.viewRadius);
            Handles.color = Color.red;
            Vector3 viewAngleA = detection.DirFromAngle(-detection.viewAngle / 2, false);
            Vector3 viewAngleB = detection.DirFromAngle(detection.viewAngle / 2, false);
            Handles.DrawLine(detection.transform.position, detection.transform.position + viewAngleA * detection.viewRadius);
            Handles.DrawLine(detection.transform.position, detection.transform.position + viewAngleB * detection.viewRadius);

            
            foreach (var visible in detection.visibleTargets)
            {
                Handles.DrawLine(detection.transform.position, visible.position);
            }
        }
    }
}
