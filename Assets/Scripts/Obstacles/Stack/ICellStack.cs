using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ICellStack {
    Transform Transform { get; }
    IEnumerable<Cell> GetCells();
    public Cell GetLastCell() => GetCells().LastOrDefault();
}