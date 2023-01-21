# Run Elastic App Search in docker
1. Configure docker resouces limits if needed (add `memory=4GB processors=2` to `.wslconfig` for `WSL 2`)
2. Set `sysctl.vm.max_map_count` to `262144` or higher if needed (add `kernelCommandLine = "sysctl.vm.max_map_count=262144"` to `.wslconfig` on `WSL 2`)
3. Set `ENCRYPTION_KEYS`, `ELASTIC_SEARCH_PASSWORD`, `KIBANA_PASSWORD`, `ENTERPRISE_SEARCH_PASSWORD` environment variables
   - `ENCRYPTION_KEYS` should have 32 symbols length or more
   - Avoid to use special characters
4. Run `docker compose --profiles setup up`
   - `setup` container runs once, other containers will start automatically at system startup
   - Use `docker compose up` or Docker Desktop to start containers again
   - If you want to run setup again, then don't forget to remove created volumes