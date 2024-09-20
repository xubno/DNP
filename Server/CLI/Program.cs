// See https://aka.ms/new-console-template for more information


using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app");
IUserRespository userRespository = new UserInMemoryRepositories();
ICommentRespository commentRespository = new CommentInMemoryRepositories();
IPostRespository postRespository = new PostInMemoryRepositories();

CliApp CliApp = new CliApp(userRespository, commentRespository, postRespository);
await CliApp.StartAsync();
