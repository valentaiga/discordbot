# discordbot

Discord bot with redis PubSub events (messages, reactions) queue.  
Hosted on [architect.io](https://architect.io/) hosting with CI/CD pipeline via **architect.yml** and **Dockerfile**s.  

## Architect.io
Architect.io provides [starter projects](https://docs.architect.io/reference/templates) for better quickstart.   

### How to configure pipeline:
1. Configure Dockerfile for your project (or multiple if required)
2. Register on [architect.io](https://architect.io/)
3. Create an environment
4. Create architect.yml file with services configuration (secrets in this file should be configured)
5. Configure secrets for architect.yml file on environment tab on architect.io website (secrets are used for deployment)
6. Create GitHub action for CI/CD purpose (like this [one](.github/workflows/architect-io-cd.yml))

