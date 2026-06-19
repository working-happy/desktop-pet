using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DesktopPet;

public partial class MainWindow : Window
{
    private readonly Random _rnd = new();
    private readonly Border _bubble;
    private readonly TextBlock _bubbleText;

    private readonly DispatcherTimer _idleTimer;
    private readonly DispatcherTimer _actionTimer;

    private readonly string[] _happyMsg = { "嘿嘿~", "好舒服！", "别挠我！", "最喜欢你了！", "汪汪！", "呜~" };
    private readonly string[] _idleMsg = { "好无聊...", "想出去玩~", "肚子饿了...", "打个哈欠~" };
    private readonly string[] _sleepMsg = { "zzz...", "呼噜呼噜..." };

    public MainWindow()
    {
        InitializeComponent();

        _bubble = (Border)FindName("Bubble")!;
        _bubbleText = (TextBlock)FindName("BubbleText")!;

        // 30秒空闲检测
        _idleTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _idleTimer.Tick += (_, _) =>
        {
            GoSleep();
        };
        _idleTimer.Start();

        // 每8秒随机动作
        _actionTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(8) };
        _actionTimer.Tick += (_, _) => { RandomAction(); };
        _actionTimer.Start();

        Loaded += (_, _) =>
        {
            // 放在屏幕底部居中
            var wa = SystemParameters.WorkArea;
            Left = (wa.Width - 140) / 2;
            Top = wa.Height - 160 - 10;
        };

        ShowBubble("你好呀~");
    }

    private void Pet_Drag(object s, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void Body_Click(object s, MouseButtonEventArgs e)
    {
        ShowBubble(_happyMsg[_rnd.Next(_happyMsg.Length)]);
        SetHappy();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void SetHappy()
    {
        // 简单的缩放弹跳效果
        var sb = new Storyboard();
        var a1 = new DoubleAnimation { To = 0.9, Duration = TimeSpan.FromMilliseconds(150) };
        Storyboard.SetTarget(a1, PetImage);
        Storyboard.SetTargetProperty(a1, new("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
        sb.Children.Add(a1);
        var a2 = new DoubleAnimation { To = 0.9, Duration = TimeSpan.FromMilliseconds(150) };
        Storyboard.SetTarget(a2, PetImage);
        Storyboard.SetTargetProperty(a2, new("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
        sb.Children.Add(a2);
        var a3 = new DoubleAnimation { To = 1.1, Duration = TimeSpan.FromMilliseconds(150), BeginTime = TimeSpan.FromMilliseconds(150) };
        Storyboard.SetTarget(a3, PetImage);
        Storyboard.SetTargetProperty(a3, new("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
        sb.Children.Add(a3);
        var a4 = new DoubleAnimation { To = 1.1, Duration = TimeSpan.FromMilliseconds(150), BeginTime = TimeSpan.FromMilliseconds(150) };
        Storyboard.SetTarget(a4, PetImage);
        Storyboard.SetTargetProperty(a4, new("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
        sb.Children.Add(a4);
        var a5 = new DoubleAnimation { To = 1.0, Duration = TimeSpan.FromMilliseconds(150), BeginTime = TimeSpan.FromMilliseconds(300) };
        Storyboard.SetTarget(a5, PetImage);
        Storyboard.SetTargetProperty(a5, new("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
        sb.Children.Add(a5);
        var a6 = new DoubleAnimation { To = 1.0, Duration = TimeSpan.FromMilliseconds(150), BeginTime = TimeSpan.FromMilliseconds(300) };
        Storyboard.SetTarget(a6, PetImage);
        Storyboard.SetTargetProperty(a6, new("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
        sb.Children.Add(a6);
        sb.Begin();
    }

    private void GoSleep()
    {
        ShowBubble(_sleepMsg[_rnd.Next(_sleepMsg.Length)]);
    }

    private void RandomAction()
    {
        switch (_rnd.Next(3))
        {
            case 0: // 弹跳
                PetImage.RenderTransform = new TranslateTransform();
                var sb1 = new Storyboard();
                var b1 = new DoubleAnimation { To = -15, Duration = TimeSpan.FromMilliseconds(300) };
                Storyboard.SetTarget(b1, PetImage);
                Storyboard.SetTargetProperty(b1, new("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                sb1.Children.Add(b1);
                var b2 = new DoubleAnimation { To = 0, Duration = TimeSpan.FromMilliseconds(300), BeginTime = TimeSpan.FromMilliseconds(300) };
                Storyboard.SetTarget(b2, PetImage);
                Storyboard.SetTargetProperty(b2, new("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                sb1.Children.Add(b2);
                sb1.Begin(PetImage);
                break;
            case 1: ShowBubble(_idleMsg[_rnd.Next(_idleMsg.Length)]); break;
            case 2: // 摇摆
                PetImage.RenderTransform = new RotateTransform();
                var sb2 = new Storyboard();
                var s1 = new DoubleAnimation { To = -10, Duration = TimeSpan.FromMilliseconds(200) };
                Storyboard.SetTarget(s1, PetImage);
                Storyboard.SetTargetProperty(s1, new("(UIElement.RenderTransform).(RotateTransform.Angle)"));
                sb2.Children.Add(s1);
                var s2 = new DoubleAnimation { To = 10, Duration = TimeSpan.FromMilliseconds(200), BeginTime = TimeSpan.FromMilliseconds(200) };
                Storyboard.SetTarget(s2, PetImage);
                Storyboard.SetTargetProperty(s2, new("(UIElement.RenderTransform).(RotateTransform.Angle)"));
                sb2.Children.Add(s2);
                var s3 = new DoubleAnimation { To = 0, Duration = TimeSpan.FromMilliseconds(200), BeginTime = TimeSpan.FromMilliseconds(400) };
                Storyboard.SetTarget(s3, PetImage);
                Storyboard.SetTargetProperty(s3, new("(UIElement.RenderTransform).(RotateTransform.Angle)"));
                sb2.Children.Add(s3);
                sb2.Begin(PetImage);
                break;
        }
    }

    private void ShowBubble(string text)
    {
        _bubbleText.Text = text;
        _bubble.Visibility = Visibility.Visible;
        _bubble.Opacity = 0;
        var fi = new DoubleAnimation { To = 1, Duration = TimeSpan.FromMilliseconds(300) };
        Storyboard.SetTarget(fi, _bubble);
        Storyboard.SetTargetProperty(fi, new("(UIElement.Opacity)"));
        var sb = new Storyboard { Children = { fi } };
        sb.Begin(_bubble);

        // 3秒后淡出
        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            var fo = new DoubleAnimation { To = 0, Duration = TimeSpan.FromMilliseconds(300) };
            fo.Completed += (_, _) => _bubble.Visibility = Visibility.Collapsed;
            Storyboard.SetTarget(fo, _bubble);
            Storyboard.SetTargetProperty(fo, new("(UIElement.Opacity)"));
            new Storyboard { Children = { fo } }.Begin(_bubble);
        };
        timer.Start();
    }
}
