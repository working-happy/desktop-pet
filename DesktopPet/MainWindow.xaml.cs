using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DesktopPet;

public partial class MainWindow : Window
{
    private enum State { Idle, Happy, Sleep }
    private State _state = State.Idle;
    private readonly Random _rnd = new();
    private int _idleSec = 0;

    private readonly Border _body;
    private readonly Ellipse _lEye, _rEye;
    private readonly Path _mouth;
    private readonly TextBlock _zzz;
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

        _body = (Border)FindName("Body")!;
        _lEye = (Ellipse)FindName("LEye")!;
        _rEye = (Ellipse)FindName("REye")!;
        _mouth = (Path)FindName("Mouth")!;
        _zzz = (TextBlock)FindName("Zzz")!;
        _bubble = (Border)FindName("Bubble")!;
        _bubbleText = (TextBlock)FindName("BubbleText")!;

        _body.RenderTransform = new TranslateTransform();

        // 30秒空闲检测
        _idleTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _idleTimer.Tick += (_, _) =>
        {
            if (++_idleSec >= 30 && _state != State.Sleep) GoSleep();
        };
        _idleTimer.Start();

        // 每8秒随机动作
        _actionTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(8) };
        _actionTimer.Tick += (_, _) => { if (_state != State.Sleep) RandomAction(); };
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

    private void Reset() => _idleSec = 0;

    private void Pet_Drag(object s, MouseButtonEventArgs e)
    {
        // 忽略关闭按钮
        var src = e.OriginalSource;
        if (src is Button || (src as FrameworkElement)?.Name == "CloseBtn") return;
        DragMove();
    }

    // 点击宠物身体触发互动
    private void Body_Click(object s, MouseButtonEventArgs e)
    {
        if (_state == State.Sleep) { WakeUp(); return; }
        SetHappy();
        ShowBubble(_happyMsg[_rnd.Next(_happyMsg.Length)]);
        Reset();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void SetHappy()
    {
        _state = State.Happy;
        _lEye.Width = 12; _lEye.Height = 6;
        _rEye.Width = 12; _rEye.Height = 6;

        var sb = new Storyboard();
        var a1 = new DoubleAnimation { To = -15, Duration = TimeSpan.FromMilliseconds(200) };
        Storyboard.SetTarget(a1, _body);
        Storyboard.SetTargetProperty(a1, new("(UIElement.RenderTransform).(TranslateTransform.Y)"));
        sb.Children.Add(a1);
        var a2 = new DoubleAnimation { To = 0, Duration = TimeSpan.FromMilliseconds(200), BeginTime = TimeSpan.FromMilliseconds(200) };
        Storyboard.SetTarget(a2, _body);
        Storyboard.SetTargetProperty(a2, new("(UIElement.RenderTransform).(TranslateTransform.Y)"));
        sb.Children.Add(a2);
        sb.Completed += (_, _) =>
        {
            _lEye.Width = 10; _lEye.Height = 10;
            _rEye.Width = 10; _rEye.Height = 10;
            _state = State.Idle;
        };
        sb.Begin();
    }

    private void GoSleep()
    {
        _state = State.Sleep;
        _lEye.Height = 2; _rEye.Height = 2;
        _mouth.Data = Geometry.Parse("M -4,18 L 4,18");
        _zzz.Visibility = Visibility.Visible;
        _body.RenderTransformOrigin = new(0.5, 0.5);
        _body.RenderTransform = new RotateTransform(90);
        ShowBubble(_sleepMsg[_rnd.Next(_sleepMsg.Length)]);
    }

    private void WakeUp()
    {
        _state = State.Idle;
        _lEye.Width = 10; _lEye.Height = 10;
        _rEye.Width = 10; _rEye.Height = 10;
        _mouth.Data = Geometry.Parse("M -4,16 Q 0,22 4,16");
        _zzz.Visibility = Visibility.Collapsed;
        _body.RenderTransform = new TranslateTransform();
        ShowBubble("醒了！");
        Reset();
    }

    private void RandomAction()
    {
        Reset();
        switch (_rnd.Next(3))
        {
            case 0: // 弹跳
                _body.RenderTransform = new TranslateTransform();
                var sb1 = new Storyboard();
                var b1 = new DoubleAnimation { To = -15, Duration = TimeSpan.FromMilliseconds(300) };
                Storyboard.SetTarget(b1, _body);
                Storyboard.SetTargetProperty(b1, new("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                sb1.Children.Add(b1);
                var b2 = new DoubleAnimation { To = 0, Duration = TimeSpan.FromMilliseconds(300), BeginTime = TimeSpan.FromMilliseconds(300) };
                Storyboard.SetTarget(b2, _body);
                Storyboard.SetTargetProperty(b2, new("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                sb1.Children.Add(b2);
                sb1.Begin(_body);
                break;
            case 1: ShowBubble(_idleMsg[_rnd.Next(_idleMsg.Length)]); break;
            case 2: // 摇摆
                _body.RenderTransform = new RotateTransform();
                var sb2 = new Storyboard();
                var s1 = new DoubleAnimation { To = -10, Duration = TimeSpan.FromMilliseconds(200) };
                Storyboard.SetTarget(s1, _body);
                Storyboard.SetTargetProperty(s1, new("(UIElement.RenderTransform).(RotateTransform.Angle)"));
                sb2.Children.Add(s1);
                var s2 = new DoubleAnimation { To = 10, Duration = TimeSpan.FromMilliseconds(200), BeginTime = TimeSpan.FromMilliseconds(200) };
                Storyboard.SetTarget(s2, _body);
                Storyboard.SetTargetProperty(s2, new("(UIElement.RenderTransform).(RotateTransform.Angle)"));
                sb2.Children.Add(s2);
                var s3 = new DoubleAnimation { To = 0, Duration = TimeSpan.FromMilliseconds(200), BeginTime = TimeSpan.FromMilliseconds(400) };
                Storyboard.SetTarget(s3, _body);
                Storyboard.SetTargetProperty(s3, new("(UIElement.RenderTransform).(RotateTransform.Angle)"));
                sb2.Children.Add(s3);
                sb2.Begin(_body);
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
            timer.Dispose();
            var fo = new DoubleAnimation { To = 0, Duration = TimeSpan.FromMilliseconds(300) };
            fo.Completed += (_, _) => _bubble.Visibility = Visibility.Collapsed;
            Storyboard.SetTarget(fo, _bubble);
            Storyboard.SetTargetProperty(fo, new("(UIElement.Opacity)"));
            new Storyboard { Children = { fo } }.Begin(_bubble);
        };
        timer.Start();
    }
}
