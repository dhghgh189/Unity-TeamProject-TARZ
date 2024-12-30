/// <summary>
/// 동작할 수 있는 인터페이스
/// </summary>
public interface IEnable
{
    /// <summary>
    /// 활성화 여부
    /// </summary>
    public bool Enable { get; set; }
    /// <summary>
    /// 스크립트의 이름
    /// </summary>
    public string Name { get; set; }
}
