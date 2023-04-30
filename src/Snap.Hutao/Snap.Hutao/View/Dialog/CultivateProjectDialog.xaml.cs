// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Model.Entity;
using Snap.Hutao.Service.User;

namespace Snap.Hutao.View.Dialog;

/// <summary>
/// ���ɼƻ��Ի���
/// </summary>
[HighQuality]
internal sealed partial class CultivateProjectDialog : ContentDialog
{
    private readonly ITaskContext taskContext;

    /// <summary>
    /// ����һ���µ����ɼƻ��Ի���
    /// </summary>
    /// <param name="serviceProvider">�����ṩ��</param>
    public CultivateProjectDialog(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        XamlRoot = serviceProvider.GetRequiredService<MainWindow>().Content.XamlRoot;

        taskContext = serviceProvider.GetRequiredService<ITaskContext>();
    }

    /// <summary>
    /// ����һ���µģ��û�ָ���ļƻ�
    /// </summary>
    /// <returns>�ƻ�</returns>
    public async ValueTask<ValueResult<bool, CultivateProject>> CreateProjectAsync()
    {
        await taskContext.SwitchToMainThreadAsync();
        ContentDialogResult result = await ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            string text = InputText.Text;
            string? uid = AttachUidBox.IsChecked == true
                ? Ioc.Default.GetRequiredService<IUserService>().Current?.SelectedUserGameRole?.GameUid
                : null;

            return new(true, CultivateProject.Create(text, uid));
        }

        return new(false, null!);
    }
}
