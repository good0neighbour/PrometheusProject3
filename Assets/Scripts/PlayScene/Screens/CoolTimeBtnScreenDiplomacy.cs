public class CoolTimeBtnScreenDiplomacy : CoolTimeBtnScreenBase
{
    private PopUpScreenDiplomacy _screen = null;

    public override void OnAdopt(byte index)
    {
        _screen.OnAdopt(index);
    }

    private void Awake()
    {
        _screen = GetComponent<PopUpScreenDiplomacy>();
    }
}
