using UnityEngine;

public class MenuManager : MonoBehaviour
{ 
    [SerializeField]
    GameObject mainMenu, settingsMenu;

    private IMenu main;
    private IMenu settings;

    public void Awake()
    {
        main = mainMenu.GetComponent<IMenu>();
        settings = settingsMenu.GetComponent<IMenu>();

        main.Setup(settings);
        settings.Setup(main);

        main.SetCallbacks();
        settings.SetCallbacks();
    }
}
