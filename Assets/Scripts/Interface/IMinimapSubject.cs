public interface IMinimapSubject
{
    /// <summary>
    /// �̴ϸ� ���� ������ ��� �޼ҵ�
    /// </summary>
    /// <param name="_observer"></param>
    void RegisterPauseObserver(IMinimapObserver _observer);

    /// <summary>
    /// �̴ϸ� ���� ������ ���� �޼ҵ�
    /// </summary>
    /// <param name="_observer"></param>
    void RemovePauseObserver(IMinimapObserver _observer);

}
