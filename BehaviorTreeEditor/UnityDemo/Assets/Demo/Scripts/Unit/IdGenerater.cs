public static class IdGenerater
{
    private static int InstanceIdGenerator;

    public static int GenerateId()
    {
        return --InstanceIdGenerator;
    }
}