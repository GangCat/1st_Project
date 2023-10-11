public interface IMyComparer<in T>
{
    int Compare(T x, T y);
}

public interface IMyComparer
{
    int Compare(int x, int y);
}