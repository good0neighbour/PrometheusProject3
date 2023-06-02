public interface ISlot
{
    /// <summary>
    /// 슬롯 클릭 시
    /// </summary>
    public void BtnTouch();

    /// <summary>
    /// 슬롯 초기화
    /// </summary>
    public void SlotInitialize(ushort index);
}
