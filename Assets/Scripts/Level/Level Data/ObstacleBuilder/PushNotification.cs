using UnityEngine;

[System.Serializable]
public class PushNotification : ObstacleBuilder {
    protected override bool ShowHorizontalIndex => false;

    [SerializeField] string notificationText;

    public override void Build(in ObstacleBuilderSettings settings) {
        NotificationRunner.Instance.Push(notificationText, PostBuildDelay);
    }
}