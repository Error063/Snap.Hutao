// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Control;
using Snap.Hutao.Model.Calculable;
using Snap.Hutao.Web.Hoyolab.Takumi.Event.Calculate;

namespace Snap.Hutao.View.Dialog;

/// <summary>
/// ���ɼ���Ի���
/// </summary>
[HighQuality]
internal sealed partial class CultivatePromotionDeltaDialog : ContentDialog
{
    private static readonly DependencyProperty AvatarProperty = Property<CultivatePromotionDeltaDialog>.Depend<ICalculableAvatar?>(nameof(Avatar));
    private static readonly DependencyProperty WeaponProperty = Property<CultivatePromotionDeltaDialog>.Depend<ICalculableWeapon?>(nameof(Weapon));

    private readonly ITaskContext taskContext;

    /// <summary>
    /// ����һ���µ����ɼ���Ի���
    /// </summary>
    /// <param name="serviceProvider">�����ṩ��</param>
    /// <param name="avatar">��ɫ</param>
    /// <param name="weapon">����</param>
    public CultivatePromotionDeltaDialog(IServiceProvider serviceProvider, ICalculableAvatar? avatar, ICalculableWeapon? weapon)
        : this(serviceProvider)
    {
        Avatar = avatar;
        Weapon = weapon;
    }

    /// <summary>
    /// ����һ���µ����ɼ���Ի���
    /// </summary>
    /// <param name="serviceProvider">�����ṩ��</param>
    /// <param name="avatar">��ɫ</param>
    public CultivatePromotionDeltaDialog(IServiceProvider serviceProvider, ICalculableAvatar? avatar)
        : this(serviceProvider)
    {
        Avatar = avatar;
    }

    /// <summary>
    /// ����һ���µ����ɼ���Ի���
    /// </summary>
    /// <param name="serviceProvider">�����ṩ��</param>
    /// <param name="weapon">����</param>
    public CultivatePromotionDeltaDialog(IServiceProvider serviceProvider, ICalculableWeapon? weapon)
        : this(serviceProvider)
    {
        Weapon = weapon;
    }

    private CultivatePromotionDeltaDialog(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        XamlRoot = serviceProvider.GetRequiredService<MainWindow>().Content.XamlRoot;

        taskContext = serviceProvider.GetRequiredService<ITaskContext>();

        DataContext = this;
    }

    /// <summary>
    /// ��ɫ
    /// </summary>
    public ICalculableAvatar? Avatar
    {
        get => (ICalculableAvatar?)GetValue(AvatarProperty);
        set => SetValue(AvatarProperty, value);
    }

    /// <summary>
    /// ����
    /// </summary>
    public ICalculableWeapon? Weapon
    {
        get => (ICalculableWeapon?)GetValue(WeaponProperty);
        set => SetValue(WeaponProperty, value);
    }

    /// <summary>
    /// �첽��ȡ��������
    /// </summary>
    /// <returns>��������</returns>
    public async Task<ValueResult<bool, AvatarPromotionDelta>> GetPromotionDeltaAsync()
    {
        await taskContext.SwitchToMainThreadAsync();
        ContentDialogResult result = await ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            AvatarPromotionDelta delta = new()
            {
                AvatarId = Avatar?.AvatarId ?? 0,
                AvatarLevelCurrent = Avatar?.LevelCurrent ?? 0,
                AvatarLevelTarget = Avatar?.LevelTarget ?? 0,
                SkillList = Avatar?.Skills.Select(s => new PromotionDelta()
                {
                    Id = s.GruopId,
                    LevelCurrent = s.LevelCurrent,
                    LevelTarget = s.LevelTarget,
                }),
                Weapon = Weapon == null ? null : new PromotionDelta()
                {
                    Id = Weapon.WeaponId,
                    LevelCurrent = Weapon.LevelCurrent,
                    LevelTarget = Weapon.LevelTarget,
                },
            };

            return new(true, delta);
        }
        else
        {
            return new(false, null!);
        }
    }
}
