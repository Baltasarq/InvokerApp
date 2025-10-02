// IncokerApp (c) 2025 MIT License <baltasarq@gmail.com>


namespace InvokerApp.Core;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class CmdLauncher {
    public const string VbleFileName = "$f";

    public required string Exe { get; init; }
    public required IList<string> Args { get; init; }

    /// <summary>Substitutes the file name and launches it.</summary>
    /// <param name="nfIn">The path that has the input file.</param>
    /// <param name="nfOut">The path that will hold the output file. Can be empty.</param>
    public void DoIt(string nfIn, string nfOut)
    {
        // Substitute file name
        for(int i = 0; i < this.Args.Count; ++i) {
            if ( this.Args[ i ].Contains( VbleFileName ) ) {
                this.Args[ i ] = this.Args[ i ].Replace( VbleFileName, nfIn );
            }
        }

        try {
            var psi = new ProcessStartInfo {
                FileName = this.Exe,
                Arguments = string.Join( " ", this.Args ),
                RedirectStandardOutput=true,
                RedirectStandardError=true
            };

            this.Output = "";
            var proc = Process.Start( psi );

            if ( proc is not null ) {
                proc.WaitForExit( -1 );
                this.Output += $"Exited: {proc.HasExited}\n";
                this.Output += $"Exit code: {proc.ExitCode}\n";
                this.Output += $"Cmd: {proc.StartInfo.FileName} {proc.StartInfo.Arguments}\n";
                this.Output += proc.StandardOutput.ReadToEnd();
                this.Output += proc.StandardError.ReadToEnd();
                this.Output += File.ReadAllText( nfOut );
            }
        } catch(Exception exc) {
            this.Output += "ERROR: " + exc.ToString();
        }
    }

    public string Output { get; private set; }  = "";
}
