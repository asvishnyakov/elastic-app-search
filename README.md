# Run Elastic App Search in docker
1. Configure docker resouces limits if needed (add `memory=4GB processors=2` to .wslconfig for WSL2)
2. Set `sysctl.vm.max_map_count` to `262144` or higher if needed (add `kernelCommandLine = "sysctl.vm.max_map_count=262144"` to .wslconfig on WSL 2)
3. Set `ENCRYPTION_KEYS`, `ELASTIC_SEARCH_PASSWORD`, `KIBANA_PASSWORD`, `ENTERPRISE_SEARCH_PASSWORD` environment variables
3.1	`ENCRYPTION_KEYS` should have 32 symbols length or more
3.2 Avoid to use special characters
4. Run `docker compose --profiles setup up`
5. If you want to run `docker compose up` again for some reason, then don't forget to remove created volumes or disable `setup`