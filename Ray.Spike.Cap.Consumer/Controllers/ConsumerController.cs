using DotNetCore.CAP;
            _logger.LogInformation("message obj is:{obj}", JsonConvert.SerializeObject(myEvt));

            Count++;
            _logger.LogInformation($"{Count}");

            if (Count < 4)
            {
                throw new Exception();

            _logger.LogInformation("-------���ս���-------");