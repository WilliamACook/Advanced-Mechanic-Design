using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Parse : MonoBehaviour
{
	[SerializeField] private string inputFile;

	public List<ParsedBlock> blocks;

	private void Awake()
	{
		//print(inputFile);
		if (!File.Exists(inputFile))
			throw new UnityException("Can't open file");
	
		//Get Blocks
		TomBenBlockParser blockParser = new TomBenBlockParser();
		blocks = blockParser.ParseFromFile(inputFile);

		//Make a parser.. pass in the blocks
		// - it spits out usable wave objects (clusters, waves, types, blah.)
	}
}
