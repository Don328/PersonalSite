using BlazorApp.Client.Utils;
using BlazorApp.Client.Utils.Constants;
using Blazored.Modal;
using System;
using Xunit;

public class ModalOptionsFactoryTests
{
    [Fact]
    public void TestGetOptions_Null()
    {
        // Tests are grouped here because null is not normally used
        // Passing null should return a default options object

        var options = ModalOptionsFactory.GetOptions(null);

        Assert.True(options.Position == ModalPosition.Center);
        Assert.True(options.Animation.Duration == ModalOptionsFactory.fadeTimer);
        Assert.True(options.HideHeader);
        Assert.False(options.ContentScrollable ?? false);
        Assert.False(options.DisableBackgroundCancel ?? false);
        Assert.False(options.HideCloseButton ?? false);
    }

    [Fact]
    public void TestGetOptions_Default_PositionIsCenter()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.True(options.Position == ModalPosition.Center);
    }

    [Fact]
    public void TestGetOptions_Default_AnimationSet()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.True(options.Animation.Duration == ModalOptionsFactory.fadeTimer);
    }

    [Fact]
    public void TestGetOptions_Default_HideHeaderIsTrue()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.True(options.HideHeader);
    }

    [Fact]
    public void TestGetOptions_Default_ContentScorllableIsFalse()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.False(options.ContentScrollable ?? false);
    }

    [Fact]
    public void TestGetOptions_Default_DisableBackgroundCancelIsFalse()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.False(options.DisableBackgroundCancel ?? false);
    }

    [Fact]
    public void TestGetOptions_Default_HideCloseButtonIsFalse()
    {
        var type = ModalTypes.Default;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.False(options.HideCloseButton ?? false);
    }

    [Fact]
    public void TestGetOptions_Scrollable_PositionIsCenter()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.True(options.Position == ModalPosition.Center);
    }

    [Fact]
    public void TestGetOptions_Scrollable_AnimationIsSet()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.True(options.Animation.Duration == ModalOptionsFactory.fadeTimer);
    }

    [Fact]
    public void TestGetOptions_Scrollable_ContentScrollableIsTrue()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.True(options.ContentScrollable);
    }

    [Fact]
    public void TestGetOptions_Scrollable_HideHeaderIsFalse()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.False(options.HideHeader ?? false);
    }

    [Fact]
    public void TestGetOptions_Scrollable_HideCloseButtonIsFalse()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.False(options.HideCloseButton ?? false);
    }

    [Fact]
    public void TestGetOptions_Scrollable_DisableBackgroundCancelIsFalse()
    {
        var type = ModalTypes.Scrollable;

        var options = ModalOptionsFactory.GetOptions(type);

        Assert.False(options.DisableBackgroundCancel ?? false);
    }

}
