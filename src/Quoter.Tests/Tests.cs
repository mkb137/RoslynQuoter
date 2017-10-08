using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

public class Tests
{
    [Fact]
    public void TestUsingSystemWithRedundantCalls()
    {
        Test(@"using System;
", @"SyntaxFactory.CompilationUnit()
.WithUsings(
    SyntaxFactory.SingletonList<UsingDirectiveSyntax>(
        SyntaxFactory.UsingDirective(
            SyntaxFactory.IdentifierName(""System""))
        .WithUsingKeyword(
            SyntaxFactory.Token(SyntaxKind.UsingKeyword))
        .WithSemicolonToken(
            SyntaxFactory.Token(SyntaxKind.SemicolonToken))))
.WithEndOfFileToken(
    SyntaxFactory.Token(SyntaxKind.EndOfFileToken))
.NormalizeWhitespace()", removeRedundantModifyingCalls: false);
    }

    [Fact]
    public void TestUsingSystemWithUsingStatic()
    {
        Test(@"using System;
", @"CompilationUnit()
.WithUsings(
    SingletonList<UsingDirectiveSyntax>(
        UsingDirective(
            IdentifierName(""System""))))
.NormalizeWhitespace()", shortenCodeWithUsingStatic: true);
    }

    [Fact]
    public void TestUsingSystem()
    {
        Test(@"using System;
", @"SyntaxFactory.CompilationUnit()
.WithUsings(
    SyntaxFactory.SingletonList<UsingDirectiveSyntax>(
        SyntaxFactory.UsingDirective(
            SyntaxFactory.IdentifierName(""System""))))
.NormalizeWhitespace()");
    }

    [Fact]
    public void TestSimpleClass()
    {
        Test("class C { }", @"SyntaxFactory.CompilationUnit()
.WithMembers(
    SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
        SyntaxFactory.ClassDeclaration(""C"")
        .WithKeyword(
            SyntaxFactory.Token(SyntaxKind.ClassKeyword))
        .WithOpenBraceToken(
            SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
        .WithCloseBraceToken(
            SyntaxFactory.Token(SyntaxKind.CloseBraceToken))))
.WithEndOfFileToken(
    SyntaxFactory.Token(SyntaxKind.EndOfFileToken))
.NormalizeWhitespace()", removeRedundantModifyingCalls: false);
    }

    [Fact]
    public void TestMissingToken()
    {
        Test("class", @"SyntaxFactory.CompilationUnit()
.WithMembers(
    SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
        SyntaxFactory.ClassDeclaration(
            SyntaxFactory.MissingToken(SyntaxKind.IdentifierToken))
        .WithKeyword(
            SyntaxFactory.Token(SyntaxKind.ClassKeyword))
        .WithOpenBraceToken(
            SyntaxFactory.MissingToken(SyntaxKind.OpenBraceToken))
        .WithCloseBraceToken(
            SyntaxFactory.MissingToken(SyntaxKind.CloseBraceToken))))
.WithEndOfFileToken(
    SyntaxFactory.Token(SyntaxKind.EndOfFileToken))
.NormalizeWhitespace()", removeRedundantModifyingCalls: false);
    }

    [Fact]
    public void TestMissingTokenWithUsingStatic()
    {
        Test("class", @"CompilationUnit()
.WithMembers(
    SingletonList<MemberDeclarationSyntax>(
        ClassDeclaration(
            MissingToken(SyntaxKind.IdentifierToken))
        .WithKeyword(
            Token(SyntaxKind.ClassKeyword))
        .WithOpenBraceToken(
            MissingToken(SyntaxKind.OpenBraceToken))
        .WithCloseBraceToken(
            MissingToken(SyntaxKind.CloseBraceToken))))
.WithEndOfFileToken(
    Token(SyntaxKind.EndOfFileToken))
.NormalizeWhitespace()", removeRedundantModifyingCalls: false, shortenCodeWithUsingStatic: true);
    }


    [Fact]
    public void TestHelloWorld()
    {
        Test(@"using System;

namespace N
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello World""); // comment
        }
    }
}", @"SyntaxFactory.CompilationUnit()
.WithUsings(
    SyntaxFactory.SingletonList<UsingDirectiveSyntax>(
        SyntaxFactory.UsingDirective(
            SyntaxFactory.IdentifierName(""System""))
        .WithUsingKeyword(
            SyntaxFactory.Token(SyntaxKind.UsingKeyword))
        .WithSemicolonToken(
            SyntaxFactory.Token(SyntaxKind.SemicolonToken))))
.WithMembers(
    SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
        SyntaxFactory.NamespaceDeclaration(
            SyntaxFactory.IdentifierName(""N""))
        .WithNamespaceKeyword(
            SyntaxFactory.Token(SyntaxKind.NamespaceKeyword))
        .WithOpenBraceToken(
            SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
        .WithMembers(
            SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                SyntaxFactory.ClassDeclaration(""Program"")
                .WithKeyword(
                    SyntaxFactory.Token(SyntaxKind.ClassKeyword))
                .WithOpenBraceToken(
                    SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                .WithMembers(
                    SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                        SyntaxFactory.MethodDeclaration(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                            SyntaxFactory.Identifier(""Main""))
                        .WithModifiers(
                            SyntaxFactory.TokenList(
                                SyntaxFactory.Token(SyntaxKind.StaticKeyword)))
                        .WithParameterList(
                            SyntaxFactory.ParameterList(
                                SyntaxFactory.SingletonSeparatedList<ParameterSyntax>(
                                    SyntaxFactory.Parameter(
                                        SyntaxFactory.Identifier(""args""))
                                    .WithType(
                                        SyntaxFactory.ArrayType(
                                            SyntaxFactory.PredefinedType(
                                                SyntaxFactory.Token(SyntaxKind.StringKeyword)))
                                        .WithRankSpecifiers(
                                            SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                                SyntaxFactory.ArrayRankSpecifier(
                                                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                        SyntaxFactory.OmittedArraySizeExpression()
                                                        .WithOmittedArraySizeExpressionToken(
                                                            SyntaxFactory.Token(SyntaxKind.OmittedArraySizeExpressionToken))))
                                                .WithOpenBracketToken(
                                                    SyntaxFactory.Token(SyntaxKind.OpenBracketToken))
                                                .WithCloseBracketToken(
                                                    SyntaxFactory.Token(SyntaxKind.CloseBracketToken)))))))
                            .WithOpenParenToken(
                                SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                            .WithCloseParenToken(
                                SyntaxFactory.Token(SyntaxKind.CloseParenToken)))
                        .WithBody(
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(""Console""),
                                                SyntaxFactory.IdentifierName(""WriteLine""))
                                            .WithOperatorToken(
                                                SyntaxFactory.Token(SyntaxKind.DotToken)))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            SyntaxFactory.Literal(""Hello World"")))))
                                            .WithOpenParenToken(
                                                SyntaxFactory.Token(SyntaxKind.OpenParenToken))
                                            .WithCloseParenToken(
                                                SyntaxFactory.Token(SyntaxKind.CloseParenToken))))
                                    .WithSemicolonToken(
                                        SyntaxFactory.Token(
                                            SyntaxFactory.TriviaList(),
                                            SyntaxKind.SemicolonToken,
                                            SyntaxFactory.TriviaList(
                                                SyntaxFactory.Comment(""// comment""))))))
                            .WithOpenBraceToken(
                                SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                            .WithCloseBraceToken(
                                SyntaxFactory.Token(SyntaxKind.CloseBraceToken)))))
                .WithCloseBraceToken(
                    SyntaxFactory.Token(SyntaxKind.CloseBraceToken))))
        .WithCloseBraceToken(
            SyntaxFactory.Token(SyntaxKind.CloseBraceToken))))
.WithEndOfFileToken(
    SyntaxFactory.Token(SyntaxKind.EndOfFileToken))
.NormalizeWhitespace()", removeRedundantModifyingCalls: false);
    }

    private void Test(
        string sourceText,
        string expected,
        bool useDefaultFormatting = true,
        bool removeRedundantModifyingCalls = true,
        bool shortenCodeWithUsingStatic = false)
    {
        var quoter = new Quoter
        {
            UseDefaultFormatting = useDefaultFormatting,
            RemoveRedundantModifyingCalls = removeRedundantModifyingCalls,
            ShortenCodeWithUsingStatic = shortenCodeWithUsingStatic
        };
        var actual = quoter.Quote(sourceText);
        Assert.Equal(expected, actual);

        Test(sourceText);
    }

    private void Test(string sourceText)
    {
        Test(sourceText, useDefaultFormatting: true, removeRedundantCalls: true, shortenCodeWithUsingStatic: false);
        Test(sourceText, useDefaultFormatting: false, removeRedundantCalls: true, shortenCodeWithUsingStatic: true);
    }

    private static void Test(string sourceText, bool useDefaultFormatting, bool removeRedundantCalls, bool shortenCodeWithUsingStatic)
    {
        if (useDefaultFormatting)
        {
            sourceText = CSharpSyntaxTree
                .ParseText(sourceText)
                .GetRoot()
                .NormalizeWhitespace()
                .ToFullString();
        }

        var quoter = new Quoter
        {
            UseDefaultFormatting = useDefaultFormatting,
            RemoveRedundantModifyingCalls = removeRedundantCalls
        };
        var generatedCode = quoter.Quote(sourceText);

        var resultText = quoter.Evaluate(generatedCode);

        if (sourceText != resultText)
        {
            //File.WriteAllText(@"D:\1.txt", sourceText);
            //File.WriteAllText(@"D:\2.txt", resultText);
            //File.WriteAllText(@"D:\3.txt", generatedCode);
        }

        Assert.Equal(sourceText, resultText);
    }
	
	[Theory]
	[MemberData( nameof( GetTestFiles ), @"Resources\RoundTrip" ) ]
	public void TestRoundtrip( string fileName ) {
		// Read the file contents
		using ( var stream = new FileStream( fileName, FileMode.Open, FileAccess.Read ) ) {
			using ( var reader = new StreamReader( stream ) ) {
				// Return the contents
				var sourceText = reader.ReadToEnd();
				Test( sourceText );
			}
		}
	}

	/// <summary>
	/// Returns the contents of every file in the given directory.
	/// </summary>
	/// <param name="directory"></param>
	/// <returns></returns>
	public static IEnumerable<object[]> GetTestFiles( string directory ) {
		// Get the directory
		var dirInfo = new DirectoryInfo( directory );
		if ( !dirInfo.Exists ) throw new DirectoryNotFoundException( directory );
		// For each file in the directory...
		foreach ( var fileInfo in dirInfo.GetFiles() ) {
			var filePath = Path.Combine( directory, fileInfo.Name );
			yield return new [] { filePath };
		}}
	}
