public interface IPoolable
{
    void OnSpawn();  // Gọi khi object được lấy từ pool
    void OnDespawn(); // Gọi khi object được trả về pool
}
