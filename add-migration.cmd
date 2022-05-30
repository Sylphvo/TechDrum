pushd Repository\Nagacasino.Repository
dotnet ef migrations add delColumnGender -v --context AppDbContext
dotnet ef database update -v --context AppDbContext
popd