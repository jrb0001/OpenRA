#region Copyright & License Information
/*
 * Copyright 2007-2019 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System;
using OpenRA.Widgets;
using System.Net;

namespace OpenRA.Mods.Common.Widgets.Logic
{
	public class DirectConnectLogic : ChromeLogic
	{
		static readonly Action DoNothing = () => { };

		[ObjectCreator.UseCtor]
		public DirectConnectLogic(Widget widget, Action onExit, Action openLobby, IPEndPoint directConnectEndpoint)
		{
			var panel = widget;
			var ipField = panel.Get<TextFieldWidget>("IP");

			ipField.Text = Game.Settings.Player.LastServer;

			panel.Get<ButtonWidget>("JOIN_BUTTON").OnClick = () =>
			{
				Game.Settings.Player.LastServer = ipField.Text;
				Game.Settings.Save();

				var endpoint = NetworkUtils.ParseEndpoint(ipField.Text);
				if (endpoint != null)
					ConnectionLogic.Connect(endpoint, "", () => { Ui.CloseWindow(); openLobby(); }, DoNothing);
			};

			panel.Get<ButtonWidget>("BACK_BUTTON").OnClick = () => { Ui.CloseWindow(); onExit(); };

			if (directConnectEndpoint != null)
			{
				// The connection window must be opened at the end of the tick for the widget hierarchy to
				// work out, but we also want to prevent the server browser from flashing visible for one tick.
				widget.Visible = false;
				Game.RunAfterTick(() =>
				{
					ConnectionLogic.Connect(directConnectEndpoint, "", () => { Ui.CloseWindow(); openLobby(); }, DoNothing);
					widget.Visible = true;
				});
			}
		}
	}
}
