
public interface IMenu
{
    public bool IsOpen();
    public void Setup(IMenu linkedMenu);
    public void SetCallbacks();
    public void Close();
    public void Open();
    public void SaveSettings();
    public void ToggleInteraction(bool interact);
}
