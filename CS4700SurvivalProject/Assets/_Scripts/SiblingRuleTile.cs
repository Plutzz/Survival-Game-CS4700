using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum TileType
{
    Grass,
    Dirt
}

[CreateAssetMenu]
public class SiblingRuleTile : RuleTile<SiblingRuleTile.Neighbor> {
    public TileType type;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Grass = 3;
        public const int Dirt = 4;
        public const int Null = 5;
        public const int NotNull = 6;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        SiblingRuleTile siblingRuleTile = tile as SiblingRuleTile;
        switch (neighbor) {
            case Neighbor.Grass:
                return siblingRuleTile?.type == TileType.Grass;
            case Neighbor.Dirt:
                return siblingRuleTile?.type == TileType.Dirt;
            case Neighbor.Null:
                return siblingRuleTile == null;
            case Neighbor.NotNull:
                return siblingRuleTile != null;
        }
        return base.RuleMatch(neighbor, tile);
    }
}