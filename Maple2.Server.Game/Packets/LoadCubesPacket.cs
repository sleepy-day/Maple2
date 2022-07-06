﻿using System.Collections.Generic;
using Maple2.Model.Common;
using Maple2.Model.Enum;
using Maple2.Model.Game;
using Maple2.PacketLib.Tools;
using Maple2.Server.Core.Constants;
using Maple2.Server.Core.Packets;
using Maple2.Tools.Extensions;

namespace Maple2.Server.Game.Packets;

public static class LoadCubesPacket {
    private enum Command : byte {
        Load = 0,
        PlotState = 1,
        LoadPlots = 2,
        PlotExpiry = 3,
    }

    public static ByteWriter Load(Plot plot) {
        var pWriter = Packet.Of(SendOp.LoadCubes);
        pWriter.Write<Command>(Command.Load);
        pWriter.WriteBool(false);
        pWriter.WriteInt(plot.Cubes.Count);
        foreach ((Vector3B position, (UgcItemCube? cube, float rotation)) in plot.Cubes) {
            pWriter.Write<Vector3B>(position);
            pWriter.WriteLong(cube.Uid);
            pWriter.WriteClass<UgcItemCube>(cube);
            pWriter.WriteInt(1);
            pWriter.WriteInt();
            pWriter.WriteBool(false);
            pWriter.WriteFloat(rotation);
            pWriter.WriteInt();
            pWriter.WriteBool(false); // Binding?
        }

        return pWriter;
    }

    public static ByteWriter PlotState(ICollection<Plot> plots) {
        var pWriter = Packet.Of(SendOp.LoadCubes);
        pWriter.Write<Command>(Command.PlotState);
        pWriter.WriteInt(plots.Count);
        foreach (Plot plot in plots) {
            pWriter.WriteInt(plot.Number);
            pWriter.Write<PlotState>(plot.State);
        }

        return pWriter;
    }

    public static ByteWriter PlotOwners(ICollection<Plot> plots) {
        var pWriter = Packet.Of(SendOp.LoadCubes);
        pWriter.Write<Command>(Command.LoadPlots);
        pWriter.WriteInt(plots.Count);
        foreach (Plot plot in plots) {
            pWriter.WriteInt(plot.Number);
            pWriter.WriteInt(plot.ApartmentNumber); // unsure
            pWriter.WriteUnicodeString(plot.Name);
            pWriter.WriteLong(plot.OwnerId);
        }

        return pWriter;
    }

    public static ByteWriter PlotExpiry(ICollection<Plot> plots) {
        var pWriter = Packet.Of(SendOp.LoadCubes);
        pWriter.Write<Command>(Command.PlotExpiry);

        pWriter.WriteInt(plots.Count);
        foreach (Plot plot in plots) {
            pWriter.WriteInt(plot.Number);
            pWriter.WriteInt(plot.ApartmentNumber); // unsure
            pWriter.Write<PlotState>(plot.State);
            pWriter.WriteLong(plot.ExpiryTime);
        }

        return pWriter;
    }
}
