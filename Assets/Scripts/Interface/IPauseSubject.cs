using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPauseSubject
{
    /// <summary>
    /// 일시정지 관련 옵저버 등록 메소드
    /// </summary>
    /// <param name="_observer"></param>
    void RegisterPauseObserver(IPauseObserver _observer);

    /// <summary>
    /// 일시정지 관련 옵저버 제거 메소드
    /// </summary>
    /// <param name="_observer"></param>
    void RemovePauseObserver(IPauseObserver _observer);

    /// <summary>
    /// 일시정지 상태 변경 메소드
    /// </summary>
    void TogglePause();
}
