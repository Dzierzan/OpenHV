#region Copyright & License Information
/*
 * Copyright 2019-2021 The OpenHV Developers (see CREDITS)
 * This file is part of OpenHV, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using OpenRA.Activities;
using OpenRA.Traits;

namespace OpenRA.Mods.HV.Activities
{
	public class FallDown : Activity
	{
		readonly IPositionable pos;
		readonly WVec fallVector;

		WPos currentPosition;

		public FallDown(Actor self, WPos currentPosition, int fallRate)
		{
			pos = self.TraitOrDefault<IPositionable>();
			IsInterruptible = false;
			fallVector = new WVec(0, 0, fallRate);
			this.currentPosition = currentPosition;
		}

		public override bool Tick(Actor self)
		{
			currentPosition -= fallVector;
			pos.SetCenterPosition(self, currentPosition);

			// If the unit has landed, this will be the last tick
			if (self.World.Map.DistanceAboveTerrain(currentPosition).Length <= 0)
			{
				var dat = self.World.Map.DistanceAboveTerrain(currentPosition);
				pos.SetPosition(self, currentPosition - new WVec(WDist.Zero, WDist.Zero, dat));

				return true;
			}

			return false;
		}

		protected override void OnFirstRun(Actor self)
		{
			// Place the actor and retrieve its visual position (CenterPosition)
			pos.SetPosition(self, currentPosition);
			currentPosition = self.CenterPosition;
		}
	}
}
