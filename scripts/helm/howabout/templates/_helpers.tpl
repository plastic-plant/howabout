{{- define "howabout.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" -}}
{{- end }}

{{- define "howabout.labels" -}}
helm.sh/chart: {{ include "howabout.chart" . }}
{{ include "howabout.selectorLabels" . }}
app.kubernetes.io/name: {{ include "howabout.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{- define "howabout.selectorLabels" -}}
app.kubernetes.io/name: {{ include "howabout.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{- define "howabout.chart" -}}
{{ .Chart.Name }}-{{ .Chart.Version }}
{{- end }}
