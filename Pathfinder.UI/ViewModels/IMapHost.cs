using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.UI.ViewModels
{
    /// <summary>
    /// Interface to remove the hard dependency between PathfinderViewModel and MapHostViewModel.
    /// This is going to be highly volatile.
    /// </summary>
    public interface IMapHost
    {

        void HideBluePin();

        void ClearMap();

        void ShowGreenPin(Core.Coordinate _target);

        void HideGreenPin();

        void ShowBluePin(Core.Coordinate from);

        void DrawLines(IEnumerable<LineViewModel> lineList);

        void ClearNodes();

        bool IsBluePinAtPosition(Core.Coordinate coordinate);

        void UpdateNode(int x, int y, bool inQueue, string text);

        void LoadWorld(Core.World<bool> world);

        void SetNodeOpenState(Core.Coordinate _coordinate, bool _state);

        Core.World<bool> World { get; }
    }
}
