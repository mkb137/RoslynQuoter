using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

public class Tests
{

	/// <summary>
	/// Tests that the source text produces the expected syntax tree.
	/// </summary>
	/// <param name="sourceText"></param>
	/// <param name="expected"></param>
	/// <param name="useDefaultFormatting"></param>
	/// <param name="removeRedundantModifyingCalls"></param>
	/// <param name="shortenCodeWithUsingStatic"></param>
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

	/// <summary>
	/// Tests that the source text, when put through the quoter and turned back into code, is unchanged.
	/// </summary>
	/// <param name="sourceText"></param>
    private void Test(string sourceText)
    {
        Test(sourceText, useDefaultFormatting: true, removeRedundantCalls: true, shortenCodeWithUsingStatic: false);
        Test(sourceText, useDefaultFormatting: false, removeRedundantCalls: true, shortenCodeWithUsingStatic: true);
    }

	/// <summary>
	/// Tests that the source text, when put through the quoter and turned back into code, is unchanged.
	/// </summary>
	/// <param name="sourceText"></param>
	/// <param name="useDefaultFormatting"></param>
	/// <param name="removeRedundantCalls"></param>
	/// <param name="shortenCodeWithUsingStatic"></param>
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
	
	/// <summary>
	/// Performs roundtrip tests on all files in the "RoundTrip" folder.
	/// </summary>
	/// <param name="fileName"></param>
	[Theory]
	[MemberData( nameof( GetFiles ), @"Resources\RoundTrip" ) ]
	public void TestRoundtrip( string fileName ) {
		// Read the file contents
		var sourceText = File.ReadAllText( fileName );
		Test( sourceText );
	}

	/// <summary>
	/// Tests syntax tree generation on all files in the "SyntaxTree" folder.
	/// 
	/// </summary>
	/// <param name="fileName"></param>
	/// <param name="syntaxFileName"></param>
	/// <param name="removeRedundantModifyingCalls"></param>
	/// <param name="shortenCodeWithUsingStatic"></param>
	[Theory]
	[MemberData( nameof( GetFilePairs ), @"Resources\SyntaxTree", ".syntax-tree", false, false ) ]
	[MemberData( nameof( GetFilePairs ), @"Resources\SyntaxTree", ".remove-redundant", true, false ) ]
	[MemberData( nameof( GetFilePairs ), @"Resources\SyntaxTree", ".using-static", false, true ) ]
	public void TestSyntaxTree( string fileName, string syntaxFileName, bool removeRedundantModifyingCalls, bool shortenCodeWithUsingStatic ) {
		// Get the source text and the syntax
		var sourceText = File.ReadAllText( fileName );
		var syntaxTree = File.ReadAllText( syntaxFileName );
		Test( 
			sourceText, 
			syntaxTree, 
			removeRedundantModifyingCalls: removeRedundantModifyingCalls,
			shortenCodeWithUsingStatic: shortenCodeWithUsingStatic 
			);
	}

	/// <summary>
	/// Returns the contents of every file in the given directory.
	/// </summary>
	/// <param name="directory"></param>
	/// <returns></returns>
	public static IEnumerable<object[]> GetFiles( string directory ) {
		// Get the directory
		var dirInfo = new DirectoryInfo( directory );
		if ( !dirInfo.Exists ) throw new DirectoryNotFoundException( directory );
		// For each file in the directory...
		foreach ( var fileInfo in dirInfo.GetFiles() ) {
			var filePath = Path.Combine( directory, fileInfo.Name );
			yield return new [] { filePath };
		}
	}

	/// <summary>
	/// Returns the contents of every file in the given directory.
	/// </summary>
	/// <param name="directory"></param>
	/// <param name="pairedFileSuffix"></param>
	/// <param name="removeRedundantModifyingCalls"></param>
	/// <param name="shortenCodeWithUsingStatic"></param>
	/// <returns></returns>
	public static IEnumerable<object[]> GetFilePairs( 
		string directory, 
		string pairedFileSuffix,
		bool removeRedundantModifyingCalls,
		bool shortenCodeWithUsingStatic 
		) {
		// Get the directory
		var dirInfo = new DirectoryInfo( directory );
		if ( !dirInfo.Exists ) throw new DirectoryNotFoundException( directory );
		// For each file in the directory...
		foreach ( var fileInfo in dirInfo.GetFiles() ) {
			// Get the file name without the extension
			var fileNameWithoutExtension = Path.GetFileNameWithoutExtension( fileInfo.Name );
			var extension = Path.GetExtension( fileInfo.Name );
			// If it's the paired file, skip it.
			if ( fileNameWithoutExtension.EndsWith( pairedFileSuffix  ) ) continue;
			// Get the file paths
			var filePath = Path.Combine( directory, fileInfo.Name );
			// Get the paired file name
			var pairedFileName = fileNameWithoutExtension + pairedFileSuffix + extension;
			// Get the paired file path
			var pairedFilePath = Path.Combine( directory, pairedFileName );
			// If it doesn't exist, stop
			if ( !File.Exists( pairedFilePath ) ) continue;
			// Return the file, paired file, and settings
			yield return new object [] { filePath, pairedFilePath, removeRedundantModifyingCalls, shortenCodeWithUsingStatic };
		}
	}
}
