using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStream
{
    public void ProcessBlocks(List<ParsedBlock> blocks)
    {
        foreach (ParsedBlock block in blocks)
        {
            if(block.type == "wave")
            {
               // print("Wave");
            }
            else if(block.type == "type")
            {
                TypeParser typeParser = new TypeParser();
                typeParser.ParsedBlock(block);
            }
            else if(block.type == "cluster")
            {

            }          
        }
    }
}
