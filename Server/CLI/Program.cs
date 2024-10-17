// See https://aka.ms/new-console-template for more information


using CLI.UI;
using FileRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app");
IUserRespository userRespository = new UserFileRepository();
ICommentRespository commentRespository = new CommentFileRepository();
IPostRespository postRespository = new PostFileRepository();

CliApp CliApp = new CliApp(userRespository, commentRespository, postRespository);
await CliApp.StartAsync();
