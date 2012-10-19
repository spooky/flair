using System;
using System.Threading.Tasks;
using System.Timers;
using Flair.Extensions;

namespace Flair.Transformations
{
/*
                                           
                    `,,`                   
               `:'''''''''';`              
            .'''''''''''''''''':           
 ......   ''''''''''''''''''''''''  ...... 
 `'''''.  '''''''':`    `:''''''';  ''''': 
  ''''';  ;''''';         :''''''. .'''''` 
  ''''''`  `''''''`      '''''''   ''''''  
  ;'''''''   ;''''';   ;''''''.  :'''''''  
  .''' ''''.  `''''''`'''''''  `''''`''';  
   '''`  ''''   ;'''''''''',  ;'''`  '''`  
   '''',   ;'',   ''''''''  `'''   `''''   
   '''''':   ;''   :'''',  '''   .''''''   
   .''',''';  .';    ''   .';  ,''':''';   
    '''` `'''; '' ``    ; ;',:''',  '''.   
    ''''.   ;''''  ': `'' '''''`  `''''    
     ''''',   :''  ''''': '';   .'''''`    
      ,''''':  ''. '''''. ''  ,''''';      
     `  '''''';:'; ''''' .'':''''''`       
     :'  :'''''''' :'''' ;''''''''  :'     
     ,''.          .''''           '''     
     `'''           ''''          ''':     
     `'''           ''''          ''',     
      ''''      `:  ''''  ',     ;'''.     
      ''''':  `'':  ''''  '''` .'''''`     
      ''''''  ''':  ''''  ''': ;'''''      
      ''''''  ''':  ''''  ''': ;'''''      
      ''''''  ''';  ''''  '''; ;'''''      
      ''''''  ''';  ''''  '''; ;'''''      
      ''''''  ''';  ''''  '''; ;'''''      
      ;'''''  ''';        '''; ;'''''      
      `'''''  ''';        '''; ;'''',      
        ''''  '''''''''''''''' ;'''.       
         '''  '''''''''''''''' ;''`        
          ''  ''''        '''' ;'          
           '  '''  :''''  .''' '           
              '''  ''''':  '''             
              ''. `''''''  ;''             
              ''  '''''''`  '              
                  ''''''''                 
*/
    public class Transformer
    {
        private ITransform transform;
        private readonly TimeSpan stall = new TimeSpan(0, 0, 1);
        private readonly Timer timer;
        private ShellViewModel shell;

        private static readonly Transformer instance = new Transformer();

        public static Transformer Instance
        {
            get { return instance; }
        }

        static Transformer()
        {
        }

        private Transformer()
        {
            timer = new Timer(stall.TotalMilliseconds);
            timer.Elapsed += OnTimerElapsed;
        }

        public void Subscribe(ShellViewModel shellViewModel)
        {
            shell = shellViewModel;
            shell.TransformChanged += OnTransformChanged;
            shell.SourceChanged += OnSourceChanged;
        }

        private void OnTransformChanged(object sender, EventArgs e)
        {
            transform = shell.ActiveTransform;
            timer.Reset();
        }

        private void OnSourceChanged(object sender, EventArgs e)
        {
            timer.Reset();
        }

        private void OnTimerElapsed(object sender, EventArgs e)
        {
            timer.Stop();
            RunAsync(shell);
        }

        private async void RunAsync(ShellViewModel shellViewModel)
        {
            if (shellViewModel == null)
                throw new ArgumentException("shellViewModel");

            if (transform == null)
                throw new InvalidOperationException("transform is null");

            shellViewModel.IsBusy = true;
            try
            {
                shellViewModel.Target = await Task.Run(() => transform.Transform(shellViewModel.Source));
            }
            catch(Exception e)
            {
                Report(e);
            }
            finally
            {
                shellViewModel.IsBusy = false;
            }
        }

        private void Report(Exception e)
        {
            shell.Target = e.Message;
        }
    }
}