namespace BuildingBlocks.Redis;

public static class RedisConfig
{
        public static string DocChunks(Guid docId) => $"doc:{docId}:chunks";
        public static string Chunk(Guid docId, int idx) => $"chunk:{docId}:{idx}";
}