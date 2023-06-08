﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Model.Metadata;
using Snap.Hutao.Service.Metadata;
using Snap.Hutao.ViewModel.AvatarProperty;
using ModelAvatarInfo = Snap.Hutao.Web.Enka.Model.AvatarInfo;

namespace Snap.Hutao.Service.AvatarInfo.Factory;

/// <summary>
/// 简述工厂
/// </summary>
[HighQuality]
[ConstructorGenerated]
[Injection(InjectAs.Transient, typeof(ISummaryFactory))]
internal sealed partial class SummaryFactory : ISummaryFactory
{
    private readonly IMetadataService metadataService;

    /// <inheritdoc/>
    public async Task<Summary> CreateAsync(IEnumerable<ModelAvatarInfo> avatarInfos, CancellationToken token)
    {
        SummaryMetadataContext metadataContext = new()
        {
            IdAvatarMap = await metadataService.GetIdToAvatarMapAsync(token).ConfigureAwait(false),
            IdWeaponMap = await metadataService.GetIdToWeaponMapAsync(token).ConfigureAwait(false),
            IdReliquaryAffixWeightMap = await metadataService.GetIdToReliquaryAffixWeightMapAsync(token).ConfigureAwait(false),
            IdReliquaryMainAffixMap = await metadataService.GetIdToReliquaryMainPropertyMapAsync(token).ConfigureAwait(false),
            IdReliquarySubAffixMap = await metadataService.GetIdToReliquarySubAffixMapAsync(token).ConfigureAwait(false),
            ReliqueryLevels = await metadataService.GetReliquaryLevelsAsync(token).ConfigureAwait(false),
            Reliquaries = await metadataService.GetReliquariesAsync(token).ConfigureAwait(false),
        };

        return new()
        {
            Avatars = avatarInfos
                .Where(a => !AvatarIds.IsPlayer(a.AvatarId))
                .Select(a => new SummaryAvatarFactory(metadataContext, a).Create())
                .OrderByDescending(a => (int)a.Quality)
                .ThenByDescending(a => a.ActivatedConstellationCount)
                .ToList(),
        };
    }
}