### How to install on Kubernetes

Docker image is not dropped in a registry like Docker Hub, but it's little effort to import it locally as shown below and rather easy to build from source. see [How to install on Docker](05_install_on_docker.md) for more details.

- See [Kubernetes deployment manifest](../../scripts/kubernetes/manifest.yaml) for Kubernetes deployment files.
- See [Helm chart](../../scripts/helm) for Helm package manager deployment files.

When you [download the source for this repository](https://github.com/plastic-plant/howabout/archive/refs/tags/v1.5.0.zip), the manifest and chart above are included in the `scripts` folder.


#### Import the image

Download the Docker image and load it into your Docker environment. Typically, downloading and a `docker load` should get you started. Once the image is loaded, you can run it with Docker or have the deployment managed by your Kubernetes cluster.

```bash
wget https://github.com/plastic-plant/howabout/releases/download/v1.5.0/howabout-1.5.0.docker.tar.gz
docker load --input howabout-1.5.0.docker.tar.gz
```


#### Install with Kubernetes

The image to deploy, a service and configmap for Howabout is defined in the [manifest.yaml](../../scripts/kubernetes/manifest.yaml) file. You can apply the manifest to your Kubernetes cluster with `kubectl apply` and delete the created resources with `kubectl delete`.
```bash
kubectl apply -f manifest.yaml
kubectl delete deployment howabout
kubeclt delete ingress howabout-ingress
kubectl delete service howabout-service
kubectl delete configmap howabout-configmap
```

Open http://localhost:5000 in your browser to see the Howabout application. If you have setup your cluster on a different host, you'll know where to find it.


#### Install with Helm

[Helm charts](https://helm.sh/docs/topics/charts/) are packages of pre-configured Kubernetes resources, typically a full application with services around a docker image. You can use Helm package manager to install and manage Kubernetes applications. See the scripts folder for the Helm chart.

```bash
cd scripts/helm
helm install howabout ./howabout
```

You can point your browser to http://localhost:5000 to demo or change the port in the [values.yaml](../../scripts/helm/values.yaml) file. Use `helm del howabout` to delete the Helm release from Kubernetes cluster.
