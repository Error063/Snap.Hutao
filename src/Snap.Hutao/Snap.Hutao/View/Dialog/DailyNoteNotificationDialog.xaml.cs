// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Model.Entity;

namespace Snap.Hutao.View.Dialog;

/// <summary>
/// ʵʱ���֪ͨ���öԻ���
/// </summary>
[HighQuality]
internal sealed partial class DailyNoteNotificationDialog : ContentDialog
{
    /// <summary>
    /// ����һ���µ�ʵʱ���֪ͨ���öԻ���
    /// </summary>
    /// <param name="serviceProvider">�����ṩ��</param>
    /// <param name="entry">ʵʱ���</param>
    public DailyNoteNotificationDialog(IServiceProvider serviceProvider, DailyNoteEntry entry)
    {
        InitializeComponent();
        XamlRoot = serviceProvider.GetRequiredService<MainWindow>().Content.XamlRoot;

        DataContext = entry;
    }
}
