using BlazorApp.Client;
using BlazorApp.Client.Utils;
using BlazorApp.Client.Utils.Enums;
using Blazored.Modal;
using System;
using Xunit;

public class ModalOptionsFactoryTests
{
    private AppState appState = new AppState();

    [Fact]
    public void TestGetOptions_Null()
    {
        // Tests are grouped here because null is not normally used
        // Passing null should return a default options object

        var options = ModalOptionsFactory.GetOptions(null, appState);

        Assert.True(options.Position == ModalPosition.Center);
        Assert.True(options.Animation.Duration == ModalOptionsFactory.fadeTimer);
        Assert.False(options.HideHeader ?? false);
        Assert.False(options.ContentScrollable ?? false);
        Assert.False(options.DisableBackgroundCancel ?? false);
        Assert.False(options.HideCloseButton ?? false);
    }

    [Fact]
    public void TestGetOptions_Default_PositionIsCenter()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.True(options.Position == ModalPosition.Center);
    }

    [Fact]
    public void TestGetOptions_Default_AnimationSet()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.True(options.Animation.Duration == ModalOptionsFactory.fadeTimer);
    }

    [Fact]
    public void TestGetOptions_Default_HideHeaderIsFalse()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.False(options.HideHeader ?? false);
    }

    [Fact]
    public void TestGetOptions_Default_ContentScorllableIsFalse()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.False(options.ContentScrollable ?? false);
    }

    [Fact]
    public void TestGetOptions_Default_DisableBackgroundCancelIsFalse()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.False(options.DisableBackgroundCancel ?? false);
    }

    [Fact]
    public void TestGetOptions_Default_HideCloseButtonIsFalse()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.False(options.HideCloseButton ?? false);
    }

    [Fact]
    public void TestGetOptions_Scrollable_PositionIsCenter()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.True(options.Position == ModalPosition.Center);
    }

    [Fact]
    public void TestGetOptions_Scrollable_AnimationIsSet()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.True(options.Animation.Duration == ModalOptionsFactory.fadeTimer);
    }

    [Fact]
    public void TestGetOptions_Scrollable_ContentScrollableIsTrue()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.True(options.ContentScrollable);
    }

    [Fact]
    public void TestGetOptions_Scrollable_HideHeaderIsFalse()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.False(options.HideHeader ?? false);
    }

    [Fact]
    public void TestGetOptions_Scrollable_HideCloseButtonIsFalse()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.False(options.HideCloseButton ?? false);
    }

    [Fact]
    public void TestGetOptions_Scrollable_DisableBackgroundCancelIsFalse()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type, appState);

        Assert.False(options.DisableBackgroundCancel ?? false);
    }

}
