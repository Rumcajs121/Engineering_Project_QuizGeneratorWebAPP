namespace UserService.Tests.FakeInfrastructure;

[CollectionDefinition("db")]
public class DbCollection:ICollectionFixture<FakeDbFixture> { }