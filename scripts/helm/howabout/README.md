# ###############################################################################
#
# helm install howabout ./howabout
# helm del howabout
#
# helm package howabout
#
# ###############################################################################

Ingress installation guide, https://kubernetes.github.io/ingress-nginx/deploy/

```
helm upgrade --install ingress-nginx ingress-nginx --repo https://kubernetes.github.io/ingress-nginx --namespace ingress-nginx --create-namespace
```