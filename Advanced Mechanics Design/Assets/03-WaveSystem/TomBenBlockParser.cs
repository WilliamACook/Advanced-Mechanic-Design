using Codice.CM.Client.Differences.Merge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class TomBenBlockParser
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

		int iter = 0;

		while(!ReachedEnd())
		{
			if (iter++ > 1000)
			{
				Debug.Log("max iters reached");
				break;
			}

			//Debug.Log(state);

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
		//string regexPattern = "(type|wave|cluster)\\s*-\\s*(\\d+)\\s*(?:\\(([\\w\\s]+)\\))?\\s*_Tom\\s*(.*?)\\s*_Ben";
		string regexPattern = @"\s*-\s*(\d+)\s*(?:\(([\w\s]+)\))?\s*_Tom";

		Regex regex = new Regex(regexPattern);

		Match regexMatch = regex.Match(charBuffer);
		//Debug.Log(charBuffer);

		if (regexMatch.Success)
		{
			for (int i = 1; i < regexMatch.Groups.Count; i++)
			{
				//Debug.Log($"Capture group {i} = {regexMatch.Groups[i]}");
			}

			currentBlock.id = int.Parse(regexMatch.Groups[1].Value);
			currentBlock.name = regexMatch.Groups[2].Value;


			//currentBlock.type = regexMatch.Groups[1].Value;
			//currentBlock.id = int.Parse(regexMatch.Groups[2].Value);
			//currentBlock.content = regexMatch.Groups[4].Value;

		}
		//TODO Move Matches
		//(type|wave|cluster)\s*- (\d+)\s*\(([\w\s]+)\)\s*_Tom [\w\s]+\s*=>(\d+)
		//Regex that works without a name
		//(type|wave|cluster)\s*-\s*(\d+)\s*(?:\(([\w\s]+)\))?\s*_Tom\s*(.*?)\s*_Ben

		ChangeState(ParserState.InsideBlockBody);
	}

	private void ParseInsideBlock()
	{
		while (!BufferHas("_Ben") && !ReachedEnd())
			NextChar();

		//reached the end or a ending brace.. so change
		//state to outside block

		string tempString = charBuffer.Trim();
		tempString = tempString[..^4];

		currentBlock.content = tempString.Trim();

		//string.IsNullOrWhiteSpace("     ");

		//Add the new block
		blocks.Add(currentBlock);
		currentBlock = new ParsedBlock();

		ChangeState(ParserState.OutsideBlock);

		//if(ReachedEnd())
		//return;
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
