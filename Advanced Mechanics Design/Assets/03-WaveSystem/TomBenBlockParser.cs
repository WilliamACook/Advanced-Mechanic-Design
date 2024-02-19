using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TomBenBlockParser : MonoBehaviour
{
	private ParsedBlock currentBlock = new ParsedBlock();
	private List<ParsedBlock> blocks = new List<ParsedBlock>();

	private enum ParserState
	{
		InsideBlockBody, InsideBlockHeader, OutsideBlock
	};

	private ParserState state = ParserState.OutsideBlock;

	//Temporary buffer used during parsing
	//Index to point of current character
	private string charBuffer = "";
	private int charIndex = 0;

	private string fileContent = "";

	private void ClearBuffer() => charBuffer = "";

	private bool ReachedEnd() => charIndex >= fileContent.Length;

	private char NextChar()
	{
		charBuffer += fileContent[charIndex];
		return fileContent[charIndex++];
	}

	private void ChangeState(ParserState state)
	{
		this.state = state;
		ClearBuffer();
	}

	private bool BufferHas(string token) => charBuffer.EndsWith(token);

	private bool BufferHasAny(params string[] tokens)
	{
		foreach(var token in tokens)
		{
			if(BufferHas(token))
				return true;
		}
		return false;
	}

	public List<ParsedBlock> ParseFromFile(string filePath)
	{
		charIndex = 0;
		charBuffer = "";
		state = ParserState.OutsideBlock;

		fileContent = File.ReadAllText(filePath);

		while(!ReachedEnd())
		{
			if (state == ParserState.OutsideBlock)
				ParseOutsideBlock();

			else if (state == ParserState.InsideBlockBody)
				ParseInsideBlock();

			else if (state == ParserState.InsideBlockHeader)
				ParseInsideBlockHeader();
		}

		return blocks;

	}

	private void ParseInsideBlockHeader()
	{
		while (!BufferHas("_Tom") && !ReachedEnd())
			NextChar();

		if (ReachedEnd())
			return;

		//TODO Create Regex for header info
		//TODO Move Matches

		ChangeState(ParserState.InsideBlockBody);
	}

	private void ParseInsideBlock()
	{
		while (!BufferHas("_Ben") && !ReachedEnd())
			NextChar();

		if(ReachedEnd())
			return;
	}

	private void ParseOutsideBlock()
	{
		while (!BufferHasAny("cluster", "type", "wave") && !ReachedEnd())
			NextChar();

		if (ReachedEnd())
			return;

		currentBlock.type = GetLastMatchedBlockType();

		ChangeState(ParserState.InsideBlockHeader);
	}

	private string GetLastMatchedBlockType()
	{
		string lastMatched = null;

		lastMatched ??= (BufferHas("cluster") ? "cluster" : null);
		lastMatched ??= (BufferHas("wave") ? "wave" : null);
		lastMatched ??= (BufferHas("type") ? "type" : null);

		return lastMatched;
	}
}
