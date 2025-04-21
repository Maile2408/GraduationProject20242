public interface ISaveable<T>
{
    T Save();
    void LoadFromSave(T data);
}
