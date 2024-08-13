namespace _ProjectBooom_.PuzzleMono.UI._3
{
    /// <summary>
    ///     回收站文件接收处UI
    /// </summary>
    public class RecycleFileReceiveUI : FileReceiveUI
    {
        public override void ReceiveFile(GhostFileUI ghostFileUI)
        {
            DestroyImmediate(ghostFileUI.SourceFileUI.gameObject);
        }
    }
}