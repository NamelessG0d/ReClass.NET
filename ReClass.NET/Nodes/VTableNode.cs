﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class VTableNode : BaseContainerNode
	{
		private readonly MemoryBuffer memory = new MemoryBuffer();

		public override int MemorySize => IntPtr.Size;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "VTable Pointer";
			icon = Properties.Resources.B16x16_Button_VTable;
		}

		public override void Intialize()
		{
			AddBytes(10 * IntPtr.Size);
		}

		public override void ClearSelection()
		{
			base.ClearSelection();

			foreach (var node in nodes)
			{
				node.ClearSelection();
			}
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(view, x, y);
			}

			DrawInvalidMemoryIndicator(view, y);

			var origX = x;
			var origY = y;

			AddSelection(view, x, y, view.Font.Height);

			x = AddOpenClose(view, x, y);
			x = AddIcon(view, x, y, Icons.VTable, -1, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.VTableColor, HotSpot.NoneId, $"VTable[{nodes.Count}]") + view.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;
			}

			x = AddComment(view, x, y);

			AddTypeDrop(view, y);
			AddDelete(view, y);

			y += view.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (LevelsOpen[view.Level])
			{
				var ptr = view.Memory.ReadIntPtr(Offset);

				memory.Size = nodes.Count * IntPtr.Size;
				memory.Process = view.Memory.Process;
				memory.Update(ptr);

				var v = view.Clone();
				v.Address = ptr;
				v.Memory = memory;

				foreach (var node in nodes)
				{
					var innerSize = node.Draw(v, tx, y);

					size.Width = Math.Max(size.Width, innerSize.Width + tx - origX);
					size.Height += innerSize.Height;

					y += innerSize.Height;
				}
			}

			return size;
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			if (IsHidden && !IsWrapped)
			{
				return HiddenHeight;
			}

			var height = view.Font.Height;
			if (LevelsOpen[view.Level])
			{
				height += nodes.Sum(n => n.CalculateDrawnHeight(view));
			}
			return height;
		}

		public override bool ReplaceChildNode(int index, Type nodeType, ref List<BaseNode> createdNodes) => false;

		public override void InsertBytes(int index, int size, ref List<BaseNode> createdNodes)
		{
			if (index < 0 || index > nodes.Count || size == 0)
			{
				return;
			}

			var offset = IntPtr.Zero;
			if (index > 0)
			{
				var node = nodes[index - 1];
				offset = node.Offset + node.MemorySize;
			}

			while (size > 0)
			{
				var node = new VMethodNode
				{
					Offset = offset,
					ParentNode = this,
					ParentNode2 = this
				};

				nodes.Insert(index, node);

				createdNodes?.Add(node);

				offset += node.MemorySize;
				size -= node.MemorySize;

				index++;
			}

			UpdateOffsets();
		}

		public override void InsertNode(int index, BaseNode node)
		{
			if (!(node is VMethodNode))
			{
				return;
			}

			base.InsertNode(index, node);

			UpdateOffsets();
		}

		public override bool RemoveNode(BaseNode node)
		{
			var removed = base.RemoveNode(node);
			if (removed)
			{
				UpdateOffsets();
			}
			return removed;
		}
	}
}
