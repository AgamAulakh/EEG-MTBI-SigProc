﻿using System;

using brainflow;
using brainflow.math;


namespace examples
{
    class SignalFiltering
    {
        static void Main (string[] args)
        {
            // use synthetic board for demo
            BoardShim.enable_dev_board_logger ();
            BrainFlowInputParams input_params = new BrainFlowInputParams ();
            int board_id = (int)BoardIds.SYNTHETIC_BOARD;

            BoardShim board_shim = new BoardShim (board_id, input_params);
            board_shim.prepare_session ();
            board_shim.start_stream (3600);
            System.Threading.Thread.Sleep (5000);
            board_shim.stop_stream ();
            double[,] unprocessed_data = board_shim.get_current_board_data (20);
            int[] eeg_channels = BoardShim.get_eeg_channels (board_id);
            board_shim.release_session ();

            for (int i = 0; i < eeg_channels.Length; i++)
            {
                DataFilter.detrend (unprocessed_data, eeg_channels[i], (int)DetrendOperations.CONSTANT);
                DataFilter.perform_bandstop (unprocessed_data, eeg_channels[i], BoardShim.get_sampling_rate (board_id), 48.0, 52.0, 4, (int)FilterTypes.BUTTERWORTH, 0.0);
                DataFilter.perform_bandpass (unprocessed_data, eeg_channels[i], BoardShim.get_sampling_rate (board_id), 4.0, 30.0, 4, (int)FilterTypes.BUTTERWORTH, 0.0);
            }
        }
    }
}
