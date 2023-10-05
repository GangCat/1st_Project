public interface IMinimapSubject
{
    /// <summary>
    /// 미니맵 관련 옵저버 등록 메소드
    /// </summary>
    /// <param name="_observer"></param>
    void RegisterPauseObserver(IMinimapObserver _observer);

    /// <summary>
    /// 미니맵 관련 옵저버 제거 메소드
    /// </summary>
    /// <param name="_observer"></param>
    void RemovePauseObserver(IMinimapObserver _observer);

}
